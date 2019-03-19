using UnityEngine;

public class MelhoraPos
{
    public static Vector3 NovaPos(Vector3 pos, float escala, string terra = "Terrain")
    {
        Vector3 retorno = pos;
        RaycastHit2D hit = Physics2D.Raycast( pos + Vector3.up, Vector3.down);

        if (hit)
        {
            terra = hit.collider.name;
            if (terra == "gambiarraSeguraQueda" || terra == "cilindroEncontro" || terra == "segundaGambiarra")
                terra = "Terrain";
            //			Debug.Log(terra+" : "+hit.collider.name);
        }

        hit = Physics2D.Raycast(pos, Vector3.down);

        if (hit)
            if (hit.transform.name == terra)
            {
                
                retorno = new Vector3(hit.point.x,hit.point.y,0) + escala * Vector3.up;


            }

        /*
        if (Physics.Raycast(pos + 0.1f * Vector3.up, Vector3.down, out hit))
            if (hit.transform.name == terra)
            {
                retorno = hit.point + escala * Vector3.up;
            }*/

        return retorno;
    }
}