using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LevanPangInterview.Controllers;
using LevanPangInterview.Gameplay;
using LevanPangInterview.Models;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace LevanPangInterview.Gameplay.Controllers
{
    public class StageController : Controller
    {
        //////////////////////////////// Members

        [Header("References")]
        [SerializeField] private Camera         GameCamera;
        [SerializeField] private SpriteRenderer BackgroundRenderer;
        [SerializeField] private TextMeshPro    TitleText;

        private Color      wallColor;
        private List<Wall> walls;

        //////////////////////////////// Properties

        public string Title
        {
            get => this.TitleText.text;
            set => this.TitleText.text = value;
        }

        public Color BackgroundColor
        {
            get => this.BackgroundRenderer.color;
            set => this.BackgroundRenderer.color = value;
        }

        public Color CameraBackgroundColor
        {
            get => this.GameCamera.backgroundColor;
            set => this.GameCamera.backgroundColor = value;
        }

        public Color WallsColor
        {
            get => this.wallColor;
            set
            {
                foreach (var wall in this.walls)
                    wall.GetComponent<SpriteRenderer>().color = value;
            }
        }

        //////////////////////////////// Lifecycle

        public override async Task OnSystemInit()
        {
            this.walls = GetComponentsInChildren<Wall>().ToList();
        }

        public void Setup()
        {
            Logger.Log($"StageController.Setup()");
        }

        internal void CleanUP()
        {
            Logger.Log($"Hero ClleanUp done");
        }
    }
}