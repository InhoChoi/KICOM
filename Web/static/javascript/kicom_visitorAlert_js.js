var previous = "";

setInterval(function() {
  makeRequest('/xml/broadcast.xml')
}, 1000);

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
  var alertMessage = "";
  var ajax = httpRequest.responseXML;
  console.log("ajax : "+ajax);
  console.log("text : "+httpRequest.responseText);

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