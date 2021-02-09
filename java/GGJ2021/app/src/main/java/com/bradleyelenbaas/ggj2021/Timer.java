package com.bradleyelenbaas.ggj2021;

import android.widget.TextView;

import java.util.Calendar;

/**
 * Counts FPS.
 */
public class Timer {
    /**
     * 1 second.
     */
    final private int PERIOD = 1000;
    /**
     * Frames counted during current second.
     */
    private int mCount = 0;
    /**
     * Time (milliseconds) since start of second.
     */
    private long mCurrent = 0;
    /**
     * Frames counted during previous second.
     */
    private int mFPS = 0;
    /**
     * Time (milliseconds) since start of pygame.
     */
    private long mNow = 0;
    /**
     * Time (milliseconds) at previous update.
     */
    private long mPrevious = 0;


    /**
     * Counts frames for 1 second, then resets.
     */
    public boolean updater() {
        mNow = Calendar.getInstance().getTime().getTime();
        mCurrent += mNow - mPrevious;
        mPrevious = mNow;
        ++mCount;
        if (mCurrent > PERIOD) {
            mFPS = mCount;
            mCount = 0;
            mCurrent = 0;
            //if (text != null) {
            //    text.setText("" + mFPS);
            //}
            return true;
        }
        return false;
    }
}
