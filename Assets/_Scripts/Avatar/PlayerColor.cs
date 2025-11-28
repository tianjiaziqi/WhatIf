using Unity.Netcode;
using UnityEngine;

public class PlayerColor : NetworkBehaviour
{
    public NetworkVariable<Color> netColor = new NetworkVariable<Color>(Color.white);
    
    public Renderer[] targetRenderer;

    public override void OnNetworkSpawn()
    {
        netColor.OnValueChanged += OnColorChanged;
        
        ApplyColor(netColor.Value);
    }

    public override void OnNetworkDespawn()
    {
        netColor.OnValueChanged -= OnColorChanged;
    }
    
    [ServerRpc]
    public void RequestColorChangeServerRpc(Color newColor)
    {
        netColor.Value = newColor;
    }

    private void OnColorChanged(Color oldColor, Color newColor)
    {
        ApplyColor(newColor);
    }

    private void ApplyColor(Color c)
    {
        if(targetRenderer != null) 
        {
            
            for (int i = 0; i < targetRenderer.Length; i++)
            {
                if (targetRenderer[i].material.HasProperty("_BaseColor"))
                {
                    targetRenderer[i].material.SetColor("_BaseColor", c);
                }
                if (targetRenderer[i].material.HasProperty("_Color"))
                {
                    targetRenderer[i].material.SetColor("_Color", c);
                }
            }
        }
    }
}
