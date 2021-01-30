import socket
import time

class Server:

    def __init__(self):
        self.localIP = "192.168.1.3"
        self.localPort = 80
        self.bufferSize = 1024

        self.msgFromServer = "Hello UDP Client"
        self.bytesToSend = str.encode(self.msgFromServer)
        self.UDPServerSocket = socket.socket(family=socket.AF_INET, type=socket.SOCK_DGRAM)
        self.UDPServerSocket.bind(('', self.localPort))

        self.percentage = ''
        self.percentageBytes = str.encode('')


    def setPercentages(self, w, h):
        self.percentage = '{},{}'.format(w, h)
        self.percentageBytes = str.encode(self.percentage)

    def server_function(self):
        bytesAddressPair = self.UDPServerSocket.recvfrom(self.bufferSize)
        message = bytesAddressPair[0]
        address = bytesAddressPair[1]

        message = str(message)
        message = message.split("'")
        message = message[1]
        message = message[0:(len(message) - 4)]

        #self.UDPServerSocket.sendto(bytesToSend, address)
        self.UDPServerSocket.sendto(self.percentageBytes, address)

        return message
