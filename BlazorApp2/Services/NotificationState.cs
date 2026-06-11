namespace BlazorApp2.Services
{
    public class NotificationState
    {
        public int UnreadCount { get; private set; }
        public List<NotificationDropdownItem> Recent { get; private set; } = new();
        public bool DropdownOpen { get; private set; }

        public event Action? OnChange;

        public void SetUnreadCount(int count)
        {
            UnreadCount = count;
            NotifyChanged();
        }

        public void Decrement()
        {
            if (UnreadCount > 0) UnreadCount--;
            NotifyChanged();
        }

        public void ClearUnread()
        {
            UnreadCount = 0;
            NotifyChanged();
        }

        public void SetRecent(List<NotificationDropdownItem> items)
        {
            Recent = items;
            NotifyChanged();
        }

        public void ToggleDropdown() { DropdownOpen = !DropdownOpen; NotifyChanged(); }
        public void CloseDropdown() { DropdownOpen = false; NotifyChanged(); }

        private void NotifyChanged() => OnChange?.Invoke();
    }

    public class NotificationDropdownItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Message { get; set; } = "";
        public string Url { get; set; } = "";
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Type { get; set; }
    }
}
