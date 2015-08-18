var previous = "";

$(document).ready(function(){
    makeRequest();
    httpRequest.onreadystatechange = setPrevious;
    httpRequest.open('GET', '/xml/broadcast.xml');
    httpRequest.send();
});

function setPrevious() {
  previous = httpRequest.responseText;
}

setInterval(function() {
  try {
        httpRequest.onreadystatechange = alertContents;
        httpRequest.open('GET', '/xml/broadcast.xml');
        httpRequest.send();
      } 
      catch (e) {}
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
}

function alertContents() {
  var alertMessage = "";
  var xmlData = httpRequest.responseXML;

  if (httpRequest.readyState == 4) {
    if (httpRequest.status == 200) {
      if (previous != httpRequest.responseText) {
        alertMessage 
          = "이름 : " + xmlData.getElementsByTagName('Name')[0].firstChild.nodeValue
          + "\n관계 : " + xmlData.getElementsByTagName('Relation')[0].firstChild.nodeValue
          + "\n시간 : " + xmlData.getElementsByTagName('Date')[0].firstChild.nodeValue;

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