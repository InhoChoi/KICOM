var previous = "";

$(document).ready(function(){
    makeRequest();
    httpRequest.onreadystatechange = setPrevious;
    httpRequest.open('GET', '/xml/broadcast.xml');
    httpRequest.send();
    console.log("리퀘스트 받음");
});

function setPrevious() {
  previous = httpRequest.responseText;
}

setInterval(function() {
  httpRequest.onreadystatechange = alertContents;
  httpRequest.open('GET', '/xml/broadcast.xml');
  httpRequest.send();
}, 1000);

function makeRequest() {
  if (window.XMLHttpRequest) { // Mozilla, Safari, ...
    httpRequest = new XMLHttpRequest();
  } else if (window.ActiveXObject) { // IE
    try {
      httpRequest = new ActiveXObject("Msxml2.XMLHTTP");
    } 
    catch (e) {
      try {
        httpRequest = new ActiveXObject("Microsoft.XMLHTTP");
      } 
      catch (e) {}
    }
  }

  if (!httpRequest) {
    alert('Giving up :( Cannot create an XMLHTTP instance');
    return false;
  }

  console.log("리퀘스트 ")
}

function alertContents() {
  var alertMessage = "";
  var ajax = httpRequest.responseXML;

  if (httpRequest.readyState == 4) {
    if (httpRequest.status == 200) {
      if (previous != httpRequest.responseText) {
        
        alertMessage 
          = "이름 : " + ajax.getElementsByTagName('Name')[0].firstChild.nodeValue
          + "\n관계 : " + ajax.getElementsByTagName('Relation')[0].firstChild.nodeValue
          + "\n시간 : " + ajax.getElementsByTagName('Date')[0].firstChild.nodeValue;

        alert(alertMessage);
        previous = httpRequest.responseText;
        console.log(alertMessage);
        console.log(previous);
        console.log(httpRequest.responseText);
      }
    }
    else {
      alert('There was a problem with the request.');
    }
  }




}