﻿using System;
using Xamarin.Forms;
using Lisa.Quartets.Mobile.Droid;
using Lisa.Quartets.Droid;
using Android.Media;
using System.IO;

[assembly: Dependency(typeof(AudioService))]

namespace Lisa.Quartets.Mobile.Droid
{
	public class AudioService : IAudio
	{
		public AudioService() {}

        public bool PlayFile(string fileName)
        {
            var resource = (int) typeof(Resource.Raw).GetField(fileName).GetValue(null);
            _mediaPlayer = MediaPlayer.Create(global::Android.App.Application.Context, resource);
            _mediaPlayer.Start();

            return true;
        }

        private MediaPlayer _mediaPlayer;
	}
}