using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderSetter : MonoBehaviour {

    private Renderer rendererRef;
    public Renderer RendererRef {
        get {
            if (rendererRef == null) {
                rendererRef = GetComponent<Renderer>();
            }
            return rendererRef;
        }
    }

    private Material material;
    public Material Material {
        get {
            if (material == null) {
                material = RendererRef.material;
            }
            return material;
        }
    }

    public string parameterName;

    public void SetTexture(Texture value) {
        Material.SetTexture(parameterName, value);
    }
    public void SetColor(Color value) {
        Material.SetColor(parameterName, value);
    }
    public void SetFloat(float value) {
        Material.SetFloat(parameterName, value);
    }

}
