public interface ISelectable
{
    bool Select();
    void Deselect();
    bool Use();
    void UpdateSelected();
    void Destroy();
}
