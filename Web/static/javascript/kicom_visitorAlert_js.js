$(document).ready(function() {
  var httpRequest;
  makeRequest('./static/xml/visitoralert.xml');

  function makeRequest(url) {
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
    
    httpRequest.onreadystatechange = alertContents;
    httpRequest.open('GET', url);
    httpRequest.send();
  }

  function alertContents() {
    if (httpRequest.readyState === 4) {
      if (httpRequest.status === 200) {
        var xmlDoc = httpRequest.responseXML;
        var alertMessage = "";
        
        alertMessage 
          = "이름 : " + xmlDoc.getElementsByTagName('Name')[0].firstChild.nodeValue
          + "\n관계 : " + xmlDoc.getElementsByTagName('Relation')[0].firstChild.nodeValue
          + "\n시간 : " + xmlDoc.getElementsByTagName('Date')[0].firstChild.nodeValue;

        alert(alertMessage)
      }
      else {
        alert('There was a problem with the request.');
      }
    }
  }
  function getData(){
    var xmlDoc = xmlhttp.responseXML;
    var DateNode = xmlDoc.getElementsByTagName('Date')[0].firstChild.nodeValue;
    alert(DateNode);
  }
});