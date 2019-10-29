function signin() {
    var open = document.getElementById('signin');
    open.style.display = 'block';
}
function resigter() {
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