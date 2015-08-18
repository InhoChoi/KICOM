#!/usr/bin/python
# -*- coding: utf-8 -*-
import sys
reload(sys)
sys.setdefaultencoding('utf-8')

from flask import Flask, render_template, request, session, jsonify, url_for, redirect, Response
import time
from threading import Thread

app = Flask(__name__)
app.config['SESSION_TYPE'] = 'memcached'
app.config['SECRET_KEY'] = 'kicom_connect'

@app.route('/')
@app.route('/login')
def login_form():
  return render_template('kicom_login.html')

@app.route('/main', methods = ['POST'])
def main():
  if request.method == 'POST':
    if(request.form['id'] == 'root'
       and request.form['password'] == '1234'):
      session['logged_in'] = True
      session['id'] = request.form['id']
      
      return render_template('kicom_main.html', id = session['id'])
    else:
      return '잘못된 접근'
  app.secret_key = 'kicom_connect'

@app.route('/home')
def home():
  return render_template('kicom_main.html', id = session['id'])

@app.route('/list')
def list():
  return render_template('kicom_list.html', id = session['id'])

@app.route('/message')
def message():
  return render_template('kicom_message.html', id = session['id'])

@app.route('/sendMessage', methods = ['POST'])
def sendMessage():
  if request.method == 'POST':
    try:
      getMessage = request.form['message_box'] + '\n'
      f = open("./messageLog.txt", 'a')
      f.write(getMessage)
      f.close()
    except Exception, e:
      getMessage = "send error"
    return render_template('kicom_send_message.html', id = session['id'], userMessage = getMessage)

@app.route('/logout')
def logout():
  session['logged_in'] = False
  session.pop('id', None)
  return render_template('kicom_login.html')

@app.route('/xml/<path:filename>')
def send_file(filename):
  try:
    filepath = './static/xml/%s' % filename
    fp = open(filepath, "rb")
    return Response(fp.read(), mimetype='text/xml')
  except Exception, e:
    return ""

if __name__ == '__main__':
  app.run(debug=True, host='0.0.0.0')