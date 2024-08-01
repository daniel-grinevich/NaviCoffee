#!/usr/bin/python
# -*- coding: utf-8 -*-
# http://elecrow.com/

import RPi.GPIO as GPIO
import time
import requests

syrupTimer = 3.0
syrupUrl = "https://moorellc.us/api/coffee/status/syruprequested"
syrupDoneUrl = "https://moorellc.us/api/coffee/status/syrupcomplete"

def checkForSyrupRequest():
    
    response = requests.get(syrupUrl)
    
    if (response.status_code == 200 and response.text != "[]"):
        # show order that syrup is being added for
        print("\nsyruping for..." + response.text)
        addSyrup()
        # mark syrup as done
        response2 = requests.put(syrupDoneUrl)    
        # show order that was updated after syrup was added
        print("\nsyrup complete: " + response2.text)        

def addSyrup():
    # define relay pin
    relay_pin = 21

    # set GPIO mode as GPIO.BOARD
    GPIO.setmode(GPIO.BCM)
    # setup relay pin as OUTPUT
    GPIO.setup(relay_pin, GPIO.OUT)


    # Open Relay
    GPIO.output(relay_pin, GPIO.LOW)
    # Wait half a second
    time.sleep(0.5)
    # Close Relay
    GPIO.output(relay_pin, GPIO.HIGH)
    # Wait half a second
    time.sleep(syrupTimer)
    # Close Relay
    GPIO.output(relay_pin, GPIO.LOW)
    GPIO.cleanup()
    
def main():
    while True:
        checkForSyrupRequest()

if __name__ == '__main__':
    main()