#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Created on Tue Mar 31 18:15:49 2020

@author: aierlande
"""
import sys

BARCODE_SCANNER_PATH = '/dev/hidraw0'
LOG_MESSAGE_PATH = '/var/log/barcodeScanner.log'
BARCODES_READ_PATH = 'barcodes.txt'

# barcode scanner translations
hid = { 4: 'a', 5: 'b', 6: 'c', 7: 'd', 8: 'e', 9: 'f', 10: 'g', 11: 'h', 12: 'i', 13: 'j', 14: 'k', 15: 'l', 16: 'm', 17: 'n', 18: 'o', 19: 'p', 20: 'q', 21: 'r', 22: 's', 23: 't', 24: 'u', 25: 'v', 26: 'w', 27: 'x', 28: 'y', 29: 'z', 30: '1', 31: '2', 32: '3', 33: '4', 34: '5', 35: '6', 36: '7', 37: '8', 38: '9', 39: '0', 44: ' ', 45: '-', 46: '=', 47: '[', 48: ']', 49: '\\', 51: ';' , 52: '\'', 53: '~', 54: ',', 55: '.', 56: '/'  }
hid2 = { 4: 'A', 5: 'B', 6: 'C', 7: 'D', 8: 'E', 9: 'F', 10: 'G', 11: 'H', 12: 'I', 13: 'J', 14: 'K', 15: 'L', 16: 'M', 17: 'N', 18: 'O', 19: 'P', 20: 'Q', 21: 'R', 22: 'S', 23: 'T', 24: 'U', 25: 'V', 26: 'W', 27: 'X', 28: 'Y', 29: 'Z', 30: '!', 31: '@', 32: '#', 33: '$', 34: '%', 35: '^', 36: '&', 37: '*', 38: '(', 39: ')', 44: ' ', 45: '_', 46: '+', 47: '{', 48: '}', 49: '|', 51: ':' , 52: '"', 53: '~', 54: '<', 55: '>', 56: '?'  }


def waitForBarcode():
    try:
        logMessage("attempting to open " + BARCODE_SCANNER_PATH)
        fp = open(BARCODE_SCANNER_PATH, 'r')
        logMessage("waiting for barcode")
        ss = ""
        shift = False
        
        done = False
        
        while not done:
        
            ## Get the character from the HID
            buffer = fp.read(8)
            for c in buffer:
                if ord(c) > 0:
        
                    ##  40 is carriage return which signifies
                    ##  we are done looking for characters
                    if int(ord(c)) == 40:
                        done = True
                        break;
        
                    ##  If we are shifted then we have to 
                    ##  use the hid2 characters.
                    if shift: 
        
                        ## If it is a '2' then it is the shift key
                        if int(ord(c)) == 2 :
                            shift = True
        
                        ## if not a 2 then lookup the mapping
                        else:
                            ss += hid2[ int(ord(c)) ]
                            shift = False
        
                    ##  If we are not shifted then use
                    ##  the hid characters
        
                    else:
        
                        ## If it is a '2' then it is the shift key
                        if int(ord(c)) == 2 :
                            shift = True
        
                        ## if not a 2 then lookup the mapping
                        else:
                            ss += hid[ int(ord(c)) ]
                    
        logMessage("Barcode scanned: " + ss)
        return ss

    except:
        fp = 'Unable to open ' + BARCODE_SCANNER_PATH + ' for reading barcode scanner. Using fake scanner for testing'
        logMessage(fp)
        return "fake barcode"

def updateFileWithNewBarcode(barcode):
    logMessage("Adding barcode to " + BARCODES_READ_PATH + ": " + barcode)
    barCodeFile = open(BARCODES_READ_PATH, 'a')
    barCodeFile.write(barcode + "\n")
    barCodeFile.close()
        

def logMessage(message):
    print(message)
    logFile = open(LOG_MESSAGE_PATH, 'a')
    logFile.write(message + "\n")
    logFile.close()
    
def main():
    while True:
        updateFileWithNewBarcode(waitForBarcode())

if __name__ == '__main__': 
    main()
    
