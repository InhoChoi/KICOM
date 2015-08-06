$(document).ready(function(){
  $.ajax({
    type: "GET",
    dataType: "xml",
    url: "static/xml/broadcast.xml",
    success: function(xml){
      var xmlData = $(xml).find("Info");
      var listLength = xmlData.length;
      if (listLength) {
        var contentStr = "";
        var location = $(this).find("Photo_Path").text();
        $(xmlData).each(function(){
          contentStr += "[이름] "+ $(this).find("Name").text()
          + "</br>[관계] " + $(this).find("Relation").text()
            + "<img src=" + $(this).find("Photo_Path").text() + "width="200" height="100">"
            + "</br>-------------------------</br>";
          });
        $("#table").append(contentStr);
      }
    }
  });
});