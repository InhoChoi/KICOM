$(document).ready(function(){
  $.ajax({
    type: "GET",
    dataType: "xml",
    url: "xml/history.xml",

    success: function(xml){
      var xmlData = $(xml).find("Info");
      var listLength = xmlData.length;
      var fTag = '<a href="';
      var bTag = '"target="_blank">사진보기</a><br>';
      var location = "static/image/History/"
      var filepath = "";
      var names = "";
      var relations = "";
      var dates = "";
      var photos = "";

      if (listLength) {
        $(xmlData).each(function(){
          filepath = $(this).find("Photo_Path").text();
          names += $(this).find("Name").text() + "<br>";
          relations += $(this).find("Relation").text() + "<br>";
          dates += $(this).find("Date").text() + "<br>"; 
          photos += fTag + location + filepath + bTag;
        });

        $("#name_content").append(names);
        $("#relation_content").append(relations);
        $("#date_content").append(dates);
        $("#photo_content").append(photos);
      }
    }
  });
});