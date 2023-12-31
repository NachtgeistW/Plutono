﻿/*
 * class Song -- Include the operation to the song.
 *
 *      This class include 
 *
 * History
 *      2020.8.12 CREATE.
 */

using System;
using System.Collections.Generic;
using Assets.Scripts.Game.Plutono;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Game.Song
{
    [System.Serializable]
    public class Song : MonoBehaviour
    {
        public SongModel Model { get; set; }

        public void LoadSongData(List<GameChart> fileChart, Image fileCover, AudioClip fileMusic, AudioClip filePreview)
        {
            Model.chart = fileChart;
            Model.cover = fileCover;
            Model.music = fileMusic;
            Model.preview = filePreview;
        }
    }
}