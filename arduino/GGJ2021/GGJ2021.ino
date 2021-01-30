/*
  UDPSendReceive.pde:
  This sketch receives UDP message strings, prints them to the serial port
  and sends an "acknowledge" string back to the sender

  A Processing sketch is included at the end of file that can be used to send
  and received messages for testing with a computer.

  created 21 Aug 2010
  by Michael Margolis

  This code is in the public domain.

  adapted from Ethernet library examples
*/


#include <ESP8266WiFi.h>
#include <WiFiUdp.h>

#ifndef STASSID
#define STASSID "NETGEAR08"
#define STAPSK  "largeskates190"
#endif

unsigned int localPort = 84;      // local port to listen on

// buffers for receiving and sending data
char packetBuffer[UDP_TX_PACKET_MAX_SIZE + 1]; //buffer to hold incoming packet,
char  ReplyBuffer[] = "acknowledged\r\n";       // a string to send back

int inByte;
String incoming;
int pin = 2;
bool toggle = false;

WiFiUDP Udp;

void setup() {
  Serial.begin(9600);
  pinMode(pin, OUTPUT);
  WiFi.mode(WIFI_STA);
  WiFi.begin(STASSID, STAPSK);
  while (WiFi.status() != WL_CONNECTED) {
    Serial.print('.');
    delay(500);
  }
  Serial.print("Connected! IP address: ");
  Serial.println(WiFi.localIP());
  Serial.printf("UDP server on port %d\n", localPort);
  Udp.begin(localPort);
}

void loop() {
  /*
  if (toggle) {
    digitalWrite(pin, HIGH);
    Serial.println("HIGH");
  } else {
    digitalWrite(pin, LOW);
    Serial.println("LOW");
  }
  toggle = !toggle;
  delay(100);
  */
  
  // if we get a valid byte, read analog ins:
  if (Serial.available() > 0) {
    // get incoming byte:
    incoming = Serial.readString();
    Serial.println(incoming);

    delay(500);
    //Serial.println("message");
    
    // if there's data available, read a packet
    int packetSize = Udp.parsePacket();
    if (packetSize) {
      Serial.printf("Received packet of size %d from %s:%d\n    (to %s:%d, free heap = %d B)\n",
                    packetSize,
                    Udp.remoteIP().toString().c_str(), Udp.remotePort(),
                    Udp.destinationIP().toString().c_str(), Udp.localPort(),
                    ESP.getFreeHeap());
  
      // read the packet into packetBufffer
      int n = Udp.read(packetBuffer, UDP_TX_PACKET_MAX_SIZE);
      packetBuffer[n] = 0;
      //Serial.println("Contents:");
      //Serial.println(packetBuffer);
  
      // send a reply, to the IP address and port that sent us the packet we received
      Udp.beginPacket(Udp.remoteIP(), Udp.remotePort());
      Udp.write(ReplyBuffer);
      Udp.endPacket();
    }
  
    Udp.beginPacket("192.168.1.6", 80);
    Udp.write(ReplyBuffer);
    Udp.endPacket();
  }
}

/*
void blink_it(int the_count){
  for (int ii=0;ii<the_count;ii++){

     digitalWrite(pin, HIGH);
     delay(150);
     digitalWrite(pin, LOW);
     delay(150);
  }
}
*/

/*
  test (shell/netcat):
  --------------------
	  nc -u 192.168.esp.address 8888
*/
