/*$(document).ready(function() {
  $("#send_button").click(function() {
    var data = $("#message_box").val();
    var result = confirm(data)
    if(result){
      $.ajax({
        type: 'POST',
        url: 'message',
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8",

        success: function(obj){
          alert('Success');
        },

        error:function(xhr,status,e){
          alert('Error');
        }
      });
    }
  });
});

/*window.onload = init;

function init() {
var button = document.getElementById("send_button"); // 아이디를 이용하여서 버튼의 정보를 얻고
button.onclick = handleButtonClick; // 버튼 클릭시에 handleButtonClick을 호출하게 함.
}

function handleButtonClick(e) 
{
    var result = confirm(document.all["message_box"].value); // 버튼을 누르면 나오게 되는 메세지.
    if(result){
      $.ajax({
        url: 'sendMessage',
        data: $('message_box').value,
        type: 'POST',

        success: function(obj){
          alert('message_box'.value);
        },

        error:function(xhr,status,e){
          alert('Error');
        }
      });
    }
}*/