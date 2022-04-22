using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RenderQuadScript))]
public class TextureSheetAnimationScript : MonoBehaviour
{
    public RenderQuadScript RenderQuad;

    private int _tilesX;
    private int _tilesY;
    private int _framesCount;
    private float _fps;

    private float _currentFrame = -1;
    private float _playStartTime;

    private void Awake()
    {
        _playStartTime = Time.time;
    }

    private void Update()
    {
        if (RenderQuad == null)
        {
            return;
        }

        if (_framesCount > 1)
        {
            int frame = (int)((Time.time - _playStartTime) * _fps);
            frame = frame % _framesCount;
            SetFrame(frame);
        }
	}

    public void SetTextureSheetData(int tilesX, int tilesY, int frames, float fps)
    {
        _tilesX = tilesX;
        _tilesY = tilesY;
        _framesCount = frames;
        _fps = fps;
        SetFrame(0);
    }

    /// <summary>
    /// Sets the frame.
    /// </summary>
    /// <param name="frame">Frame.</param>
    public void SetFrame(int frame)
    {
        if (frame == _currentFrame)
        {
            return;
        }

        float xUnitSize = 1.0f / this._tilesX;
        float yUnitSize = 1.0f / this._tilesY;

        int xIndex = frame % _tilesX;
        int yIndex = frame / _tilesX;
        yIndex = _tilesY - yIndex - 1;

        Vector2[] uv = new Vector2[] {
            new Vector2(xIndex * xUnitSize, yIndex * yUnitSize),
            new Vector2(xIndex * xUnitSize, yIndex * yUnitSize) + new Vector2(xUnitSize, 0),
            new Vector2(xIndex * xUnitSize, yIndex * yUnitSize) + new Vector2(0, yUnitSize),
            new Vector2(xIndex * xUnitSize, yIndex * yUnitSize) + new Vector2(xUnitSize, yUnitSize),
        };
        this.RenderQuad.MeshFilter.mesh.uv = uv;
        _currentFrame = frame;
    }
}
