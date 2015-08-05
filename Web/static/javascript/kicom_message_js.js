window.onload = init;

function init() {
var button = document.getElementById("send_button"); // 아이디를 이용하여서 버튼의 정보를 얻고
button.onclick = handleButtonClick; // 버튼 클릭시에 handleButtonClick을 호출하게 함.
}

function handleButtonClick(e) 
{
    alert(document.all["message_box"].value); // 버튼을 누르면 나오게 되는 메세지.
}