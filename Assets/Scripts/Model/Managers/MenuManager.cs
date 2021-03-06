﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

namespace Model.Managers
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager instance;
        private void Awake()
        {
            instance = this;
        }
        public void StartGame()
        {
            SceneLoader.GotoPrepareScene();
        }

        public void GotoMain()
        {
            GameManager.Reset();
            SceneLoader.GotoMain();
        }

        public void GotoStage()
        {
            SceneLoader.GotoStage();
        }

        public void ExitGame()
        {
            Application.Quit();
        }

    }
}