using UnityEngine;

public static class BondDrawer
{
    public static GameObject DrawBond(
        Vector3 start,
        Vector3 end,
        Material bondMaterial,
        float thickness = 0.08f,
        Transform parent = null)
    {

        GameObject bond = GameObject.CreatePrimitive(
            PrimitiveType.Cylinder);
        if (parent != null)
            bond.transform.SetParent(parent);
        Object.Destroy(
            bond.GetComponent<Collider>());
        bond.transform.position =
            (start + end) / 2f;
        bond.transform.up = (end - start).normalized;
        float distance = Vector3.Distance(start, end);
        bond.transform.localScale = new Vector3(
            thickness,
            distance / 2f,
            thickness);
        if (bondMaterial != null)
            bond.GetComponent<Renderer>()
                .material = bondMaterial;
        bond.name = "Bond";
        return bond;
    }
}