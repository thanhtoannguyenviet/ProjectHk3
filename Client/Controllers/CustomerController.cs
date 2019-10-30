using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Client.Models;
using Newtonsoft.Json;
using PayPal.Api;
using static Client.Service.CustomerService;
namespace Client.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer

        [Authorize(Roles = "Customer")]
        public ActionResult Index()
        {
            if (Session["cart"] != null)
            {
                List<Detail> ls = Session["cart"] as List<Detail>;
            }
            ViewBag.Count = CountStaffCustomer() / 3;
            return View(GetStaffCustomer(0));
        }

        [Authorize(Roles = "Customer")]
        public ActionResult AddToCart(string staffId, string dateSTT, string dateEND, string amountMoney)
        {
            Detail de = new Detail() { staffId = Int32.Parse(staffId), startDate = DateTime.Parse(dateSTT), endDate = DateTime.Parse(dateEND), amountMoney = decimal.Parse(amountMoney) };
            var ls = new List<Detail>();
            if (Session["cart"] != null)
            {
                ls = Session["cart"] as List<Detail>;
            }

            ls.Add(de);
            Session["cart"] = ls;
            return RedirectToAction("Index", "Home");
        }
        private PayPal.Api.Payment payment;
        [Authorize(Roles = "Customer")]
        public ActionResult Pay()
        {
            List<Detail> ls = Session["cart"] as List<Detail>;
            APIContext apiContext = PayPalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Payment/Pay?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid, ls);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    //Session.Add(guid, createdPayment.id);
                    TempData[guid] = createdPayment.id;
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    //var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    var executedPayment = ExecutePayment(apiContext, payerId, TempData[guid] as string);
                    // add detailpayment model
                    var lsDetails = new List<Detail>();
                    var cus = Session["Account"] as AccountCustomer;
                    var pay = new Client.Models.Payment()
                    {
                        paymentId = executedPayment.id,
                        totalMoney = decimal.Parse(executedPayment.transactions[0].amount.total)
                    };
                    foreach (var item in ls)
                    {
                        var detail = new Detail()
                        {
                            customerId = cus.id,
                            staffId = item.staffId,
                            startDate = item.startDate,
                            endDate = item.endDate,
                            amountMoney = item.amountMoney,
                            statusOrder = 0,
                            createDate = DateTime.Today
                        };
                        lsDetails.Add(detail);
                    };
                    var detailPayment = new DetailPayment()
                    {
                        details = lsDetails,
                        payment = pay,
                        customer = cus.customer
                    };
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        Session.Remove("cart");
                        return JavaScript("Payment Error!");
                    }
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Clear();
                        var response = client.PostAsync("http://localhost:61143/api/payment/createOrder/", new StringContent(
                            new JavaScriptSerializer().Serialize(detailPayment), Encoding.UTF8, "application/json")).Result;
                        if (response.StatusCode != HttpStatusCode.OK)
                            return JavaScript("Payment API Error!");
                    }
                }
            }
            catch (Exception ex)
            {
                Session.Remove("cart");
                return JavaScript("Payment Error! " + ex.Message);
            }
            Session.Remove("cart");
            //on successful payment, show success page to user.
            return JavaScript("Payment Success!");
        }

        private PayPal.Api.Payment CreatePayment(APIContext apiContext, string redirectUrl, List<Detail> ls)
        {
            //create itemlist and add item objects to it
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc
            decimal totalAll = 0;
            foreach (var item in ls)
            {
                itemList.items.Add(new Item()
                {
                    name = item.staffId.ToString(),
                    currency = "USD",
                    price = (item.amountMoney / decimal.Parse(((item.endDate.Value - item.startDate.Value).TotalDays + 1).ToString())).ToString(),
                    quantity = ((item.endDate.Value - item.startDate.Value).TotalDays + 1).ToString()
                });
                totalAll += item.amountMoney.Value;
            };
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = Request.Url.Scheme + "://" + Request.Url.Authority + "/Home/Index",
                return_url = redirectUrl
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = totalAll.ToString()
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            transactionList.Add(new Transaction()
            {
                description = "Transaction description",
                invoice_number = Client.Models.Common.GetRandomInvoiceNumber(), //Generate an Invoice No  
                amount = amount,
                item_list = itemList
            });
            this.payment = new PayPal.Api.Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }

        private PayPal.Api.Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new PayPal.Api.Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        [Authorize(Roles = "Customer")]
        public ActionResult RemoveFromCart(string index)
        {
            var ls = Session["cart"] as List<Detail>;
            if (ls != null) ls.RemoveAt(Int32.Parse(index) - 1);
            return RedirectToAction("Index");
        }
        public ActionResult StaffCanHire(int id)
        {
            return PartialView("_StaffCanHire", GetStaffCustomer(id));
        }
        [Authorize(Roles = "Customer")]
        public ActionResult Order()
        {
            var session = Session["Account"]  as AccountCustomer;
            var customer = session.customer;
            ViewBag.Count = CountAllOrder(customer)/3;
            return View(GetAllOrder(customer, 0));
        }

        [Authorize(Roles = "Customer")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Order(int id)
        {
            var customer = Session["Account"] as Customer;
            ViewBag.Count = CountAllOrder(customer);
            return View(GetAllOrder(customer, id));
        }

        [Authorize(Roles = "Customer")]
        public ActionResult Payment()
        {
            var customer = Session["Account"] as Customer;
            ViewBag.Count = CountPayment(customer);
            return View(GetPayment(customer,0));
        }

        [Authorize(Roles = "Customer")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Payment(int id)
        {
            var customer = Session["Account"] as Customer;
            ViewBag.Count = CountPayment(customer);
            return View(GetPayment(customer, id));
        }

        public ActionResult SettingView()
        {
            AccountCustomer accountCustomer = Session["Account"] as AccountCustomer;
            
            return View(accountCustomer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SettingView(AccountCustomer accountCustomer)
        {
            var newAccountCustomer = UpdateCustomer(accountCustomer);
            if (newAccountCustomer != null)
            {
                return Index();
            }
            return View(accountCustomer);
        }
        //detailStaff
        [HttpPost]
        public ActionResult Img(HttpPostedFileBase ImageFile)
        {
            AccountCustomer accountCustomer = Session["Account"] as AccountCustomer;
            if (ImageFile.ContentLength > 0) {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                ImageFile.SaveAs(path);
            accountCustomer.img.path_ = "~/Images/" + fileName;
            var img = UpdateImg(accountCustomer.img);
            if (img != null)    
            {
                accountCustomer.img = img;
                Session["Account"] = accountCustomer;
                ViewBag.Count = CountStaffCustomer() / 3;
                ViewBag.UpdateSuccess = 1;
                return View("Index",GetStaffCustomer(0));
            }
            }
            return SettingView();
        }
    }
}