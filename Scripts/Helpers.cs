using Godot;

static class Helpers
{
    public static bool Near(float x, float y)
    {
        float diff = Mathf.Abs(x - y);
        return diff < 0.0001f;
    }
}