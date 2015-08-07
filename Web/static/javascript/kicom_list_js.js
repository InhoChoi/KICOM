$(document).ready(function(){
  $.ajax({
    type: "GET",
    dataType: "xml",
    url: "static/xml/broadcast.xml",

    success: function(xml){
      var xmlData = $(xml).find("Info");
      var listLength = xmlData.length;
      var fTag = '<a href="';
      var bTag = '">사진보기</a><br>';
      var names = "";
      var relations = "";
      var dates = "";
      var photos = "";

      if (listLength) {
        var location = $(this).find("Photo_Path").text();
        
        $(xmlData).each(function(){
          names += $(this).find("Name").text() + "<br>";
          relations += $(this).find("Relation").text() + "<br>";
          dates += $(this).find("Date").text() + "<br>"; 
          photos += fTag + $(this).find("Photo_Path").text() + bTag;
        });

        $("#name_content").append(names);
        $("#relation_content").append(relations);
        $("#date_content").append(dates);
        $("#photo_content").append(photos);
      }
    }
  });
});