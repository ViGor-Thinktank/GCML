using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuclearWinter.GameFlow
{
    public interface IGameState<out T> where T:NuclearGame
    {
        void Start();
        void Stop();
        void OnActivated();
        void OnExiting();
        bool UpdateFadeIn( float _fElapsedTime );
        void DrawFadeIn();
        bool UpdateFadeOut( float _fElapsedTime );
        void DrawFadeOut();
        void Update( float _fElapsedTime );
        void Draw();

    }
}
