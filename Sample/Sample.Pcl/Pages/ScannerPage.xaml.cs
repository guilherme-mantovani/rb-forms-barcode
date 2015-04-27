﻿using Xamarin.Forms;
using Rb.Forms.Barcode.Pcl;

namespace Sample.Pcl.Pages
{
    public partial class ScannerPage : ContentPage
    {
        public ScannerPage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);


            /**
             * So that we can release the camera when turning off phone or switching apps.
             */
            MessagingCenter.Subscribe<App>(this, App.MessageOnSleep, disableScanner);
            MessagingCenter.Subscribe<App>(this, App.MessageOnResume, enableScanner);

            barcodeScanner.BarcodeChanged += animateFlash;
        }

        /**
         * Release camera so that other apps can access it.
         */
        private void disableScanner(object sender)
        {
            barcodeScanner.IsEnabled = false;
        }

        /**
         * All your camera belongs to us.
         */
        private void enableScanner(object sender)
        {
            barcodeScanner.IsEnabled = true;
        }

        private async void animateFlash(object sender, BarcodeEventArgs e)
        {
            await flash.FadeTo(1, 150, Easing.CubicIn);
            await flash.FadeTo(0, 150, Easing.CubicOut);            
        }

        /**
         * You need to take care of realeasing the camera when you are done with it else bad things can happen!
         */
        ~ScannerPage()
        {
            disableScanner(this);

            /**
             * Camera is released we dont need the events anymore.
             */
            MessagingCenter.Unsubscribe<App>(this, App.MessageOnSleep);
            MessagingCenter.Unsubscribe<App>(this, App.MessageOnResume);

            barcodeScanner.BarcodeChanged -= animateFlash;
        }
    }
}

