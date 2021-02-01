package com.bradleyelenbaas.ggj2021;

import androidx.appcompat.app.AppCompatActivity;

import android.graphics.Color;
import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;

import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.text.DateFormat;
import java.util.Date;

public class MainActivity extends AppCompatActivity {

    private CanvasView customCanvas;

    String message = "0.0,0.0";
    final String IPAdress = "192.168.1.70";

    Button button;
    int counter = 0;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        customCanvas = (CanvasView) findViewById(R.id.signature_canvas);

        button = findViewById(R.id.button);
        button.setTextColor(Color.BLACK);
        button.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                MessageSender messageSender = new MessageSender();
                messageSender.execute();
                //button.setText("counter: " + counter++);
            }
        });
        button.setBackgroundColor(Color.WHITE);

        startServerSocket();
    }

    public void clearCanvas(View v) {
        customCanvas.clearCanvas();
    }

    private void startServerSocket() {

        Thread thread = new Thread(new Runnable() {

            private String stringData = null;

            @Override
            public void run() {

                byte[] msg = new byte[1000];
                DatagramPacket dp = new DatagramPacket(msg, msg.length);
                DatagramSocket ds = null;
                try {
                    ds = new DatagramSocket(50007);
                    //ds.setSoTimeout(50000);
                    ds.receive(dp);

                    stringData = new String(msg, 0, dp.getLength());
                    if (stringData.contains("on"))
                    {
                        button.setBackgroundColor(Color.RED);
                        message = "on";
                    }
                    else if (stringData.contains("off"))
                    {
                        button.setBackgroundColor(Color.WHITE);
                        message = "off";
                    }
                    String currentDateTimeString = DateFormat.getDateTimeInstance().format(new Date());
                    stringData = stringData + " " + currentDateTimeString;
                    //button.setText(stringData);
                    Log.d("SOMETHING", stringData);

                    MessageSender messageSender = new MessageSender();
                    messageSender.execute();

                    //updateUI(stringData);

                    //String msgToSender = "Bye Bye ";
                    //dp = new DatagramPacket(msgToSender.getBytes(), msgToSender.length(), dp.getAddress(), dp.getPort());
                    //ds.send(dp);

                } catch (IOException e) {
                    e.printStackTrace();
                } finally {
                    if (ds != null) {
                        ds.close();
                        startServerSocket();
                    }
                }
            }

        });
        thread.start();
    }

    private class MessageSender extends AsyncTask<String, Void, Void> {
        @Override
        protected Void doInBackground(String... voids) {

            int serverPort = 80;

            try {
                DatagramSocket s = new DatagramSocket();
                InetAddress inetAddress = InetAddress.getByName(IPAdress);
                byte[] tempMessage = message.getBytes();
                DatagramPacket d = new DatagramPacket(
                        tempMessage,
                        message.length(),
                        inetAddress,
                        serverPort
                );
                s.send(d);
                //Log.d("SOMETHING", "" + d);
            } catch (Exception e) {
                System.err.println(e);
                e.printStackTrace();
            }

            return null;
        }
    }
}