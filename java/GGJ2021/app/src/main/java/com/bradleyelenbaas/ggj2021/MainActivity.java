package com.bradleyelenbaas.ggj2021;

import androidx.appcompat.app.AppCompatActivity;

import android.graphics.Color;
import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import android.view.MotionEvent;
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

    String message = "android,up";
    final String IPAdress = "192.168.1.70";
    byte[] msg = new byte[1000];

    Button buttonUp;
    Button buttonDown;
    Button buttonLeft;
    Button buttonRight;
    Button buttonStop;
    int counter = 0;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        buttonUp = findViewById(R.id.button_up);
        buttonUp.setOnTouchListener(new View.OnTouchListener() {
            @Override
            public boolean onTouch(View v, MotionEvent event) {
                if(event.getAction() == MotionEvent.ACTION_DOWN) {
                    message = "android,up";
                    MessageSender messageSender = new MessageSender();
                    messageSender.execute();
                } else if (event.getAction() == MotionEvent.ACTION_UP) {
                    message = "";
                    MessageSender messageSender = new MessageSender();
                    messageSender.execute();
                }
                return true;
            }
        });
        buttonDown = findViewById(R.id.button_down);
        buttonDown.setOnTouchListener(new View.OnTouchListener() {
            @Override
            public boolean onTouch(View v, MotionEvent event) {
                if(event.getAction() == MotionEvent.ACTION_DOWN) {
                    message = "android,down";
                    MessageSender messageSender = new MessageSender();
                    messageSender.execute();
                } else if (event.getAction() == MotionEvent.ACTION_UP) {
                    message = "";
                    MessageSender messageSender = new MessageSender();
                    messageSender.execute();
                }
                return true;
            }
        });
        buttonLeft = findViewById(R.id.button_left);
        buttonLeft.setOnTouchListener(new View.OnTouchListener() {
            @Override
            public boolean onTouch(View v, MotionEvent event) {
                if(event.getAction() == MotionEvent.ACTION_DOWN) {
                    message = "android,left";
                    MessageSender messageSender = new MessageSender();
                    messageSender.execute();
                } else if (event.getAction() == MotionEvent.ACTION_UP) {
                    message = "";
                    MessageSender messageSender = new MessageSender();
                    messageSender.execute();
                }
                return true;
            }
        });
        buttonRight = findViewById(R.id.button_right);
        buttonRight.setOnTouchListener(new View.OnTouchListener() {
            @Override
            public boolean onTouch(View v, MotionEvent event) {
                if(event.getAction() == MotionEvent.ACTION_DOWN) {
                    message = "android,right";
                    MessageSender messageSender = new MessageSender();
                    messageSender.execute();
                } else if (event.getAction() == MotionEvent.ACTION_UP) {
                    message = "";
                    MessageSender messageSender = new MessageSender();
                    messageSender.execute();
                }
                return true;
            }
        });

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
                //byte[] msg = new byte[1000];
                DatagramPacket dp = new DatagramPacket(msg, msg.length);
                DatagramSocket ds = null;
                try {
                    ds = new DatagramSocket(50007);
                    ds.receive(dp);
                    stringData = new String(msg, 0, dp.getLength());
                    /*
                    if (stringData.contains("on")) {
                        button.setBackgroundColor(Color.RED);
                        message = "on";
                    } else if (stringData.contains("off")) {
                        button.setBackgroundColor(Color.WHITE);
                        message = "off";
                    }
                    */
                    String currentDateTimeString = DateFormat.getDateTimeInstance().format(new Date());
                    stringData = stringData + " " + currentDateTimeString;
                    MessageSender messageSender = new MessageSender();
                    messageSender.execute();
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

            int serverPort = 50007;

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