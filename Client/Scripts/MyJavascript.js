function signin() {
    document.getElementById('resigter').style.display = 'none';
    var open = document.getElementById('signin');
    open.style.display = 'block';
}
function resigter() {
    document.getElementById('signin').style.display = 'none';
    var open = document.getElementById('resigter');
    open.style.display = 'block';
}

function getLogin() {
    var account = {
        userName: $('#userName').val(),
        pass_word: $('#pass_word').val()
    };
    $.ajax({
        url: 'http://localhost:61143/api/account/Login/',
        method: 'POST',
        dataType:'json',
        contextType: 'application/json',
        data:JSON.stringify(account),
        success: function (response) {
            alert("Hello! I am an alert box!!");
            sessionStorage.setItem('accessToken', response.access_token);
        },
        error: function(xhr) {
            alert(xhr.responseText);
        }
    })
}
function login() {
    var response = client.GetStringAsync(url);
    var rootObject = JsonConvert.DeserializeObject<RootObject>(response.Result);
}

// Get the Sidebar
var mySidebar = document.getElementById("mySidebar");

// Get the DIV with overlay effect
var overlayBg = document.getElementById("myOverlay");

// Toggle between showing and hiding the sidebar, and add overlay effect
function w3_open() {
    if (mySidebar.style.display === 'block') {
        mySidebar.style.display = 'none';
        overlayBg.style.display = "none";
    } else {
        mySidebar.style.display = 'block';
        overlayBg.style.display = "block";
    }
}

// Close the sidebar with the close button
function w3_close() {
    mySidebar.style.display = "none";
    overlayBg.style.display = "none";
}

function changeImage(input) {
    var ext = input.files[0]['name'].substring(input.files[0]['name'].lastIndexOf('.') + 1).toLowerCase();
    if (input.files && input.files[0] && (ext == "gif" || ext == "png" || ext == "jpeg" || ext == "jpg")) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#Image').attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
    else {
        $('#Image').attr('src', '/Image/jpg.png');
    }
}
function CheckInfo(element, type) {
    var regexp;
    var message;
    switch (type) {
    case "phone":
         regexp = /^[0-9]$/;
         message = 'Please Enter Number Only';
         break;
    case "Confpassword":
         regexp = $('#Confpassword').val;
            if (regexp == element.val()) {
                message = 'Please Enter Number Only';
            }
            break;
    default :
    }
    if (message != null) {
        element.placeholder = message;
        element.classList.add('border border-dangerous');
    } else {
        element.classList.add('border border-success');
    }
}