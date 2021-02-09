package com.bradleyelenbaas.ggj2021;

import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.util.AttributeSet;
import android.view.MotionEvent;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

public class CanvasView extends View {

    private static int BOX_OFFSET = 100;
    public int width;
    public int height;
    private Bitmap mBitmap;
    private Canvas mCanvas;
    Context context;
    private Paint mPaintBackground;
    private Paint mPaintCloud;
    private float mX, mY;
    private static final float TOLERANCE = 5;

    //private Timer mTimer;
    //Button button;

    public CanvasView(Context c, AttributeSet attrs) {
        super(c, attrs);
        context = c;

        mPaintBackground = new Paint();
        mPaintBackground.setColor(Color.parseColor("#38491a"));
        mPaintCloud = new Paint();
        mPaintCloud.setAntiAlias(true);
        mPaintCloud.setColor(Color.BLACK);
        mPaintCloud.setStyle(Paint.Style.FILL);

        //mTimer = new Timer();
        //button = findViewById(R.id.button);
    }

    @Override
    protected void onSizeChanged(int w, int h, int oldw, int oldh) {
        super.onSizeChanged(w, h, oldw, oldh);

        mBitmap = Bitmap.createBitmap(w, h, Bitmap.Config.ARGB_8888);
        mCanvas = new Canvas(mBitmap);
    }

    @Override
    protected void onDraw(Canvas canvas) {
        super.onDraw(canvas);
        //if (mTimer.updater()) {
        //    button.setText("---");
        //}
        canvas.drawRect(
                0,
                0,
                canvas.getWidth(),
                canvas.getHeight(),
                mPaintBackground
        );
        canvas.drawRect(
                0,
                0,
                canvas.getWidth(),
                mY - BOX_OFFSET,
                mPaintCloud
        );
        canvas.drawRect(
                0,
                mY + BOX_OFFSET,
                canvas.getWidth(),
                canvas.getHeight(),
                mPaintCloud
        );
        canvas.drawRect(
                0,
                mY - BOX_OFFSET,
                mX - BOX_OFFSET,
                mY + BOX_OFFSET,
                mPaintCloud
        );
        canvas.drawRect(
                mX + BOX_OFFSET,
                mY - BOX_OFFSET,
                canvas.getWidth(),
                mY + BOX_OFFSET,
                mPaintCloud
        );

        //canvas.drawPath(mPath, mPaint);
    }

    private void startTouch(float x, float y) {
        mX = x;
        mY = y;
    }

    private void moveTouch(float x, float y) {
        float dx = Math.abs(x - mX);
        float dy = Math.abs(y - mY);
        if (dx >= TOLERANCE || dy >= TOLERANCE) {
            mX = x;
            mY = y;
        }
    }

    public void clearCanvas() {
        invalidate();
    }

    private void upTouch() {
    }

    @Override
    public boolean onTouchEvent(MotionEvent event) {
        float x = event.getX();
        float y = event.getY();

        switch (event.getAction()) {
            case MotionEvent.ACTION_DOWN:
                startTouch(x, y);
                invalidate();
                break;
            case MotionEvent.ACTION_MOVE:
                moveTouch(x, y);
                invalidate();
                break;
            case MotionEvent.ACTION_UP:
                upTouch();
                invalidate();
                break;
        }
        return true;
    }
}
