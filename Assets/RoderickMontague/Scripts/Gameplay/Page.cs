using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    public class Page
    {
        // The text for the page.
        public string text = "";

        // The callback for the page.
        public delegate void PageCallback();

        // The callback for opening the page.
        private PageCallback pageOpenCallback;

        // The callback for closing the page.
        private PageCallback pageCloseCallback;

        // Empty page.
        public Page()
        {
            text = "";
        }

        // Adds a page with text.
        public Page(string text)
        {
            this.text = text;
        }

        // Add a callback for when the page is opened.
        public void OnPageOpenedAddCallback(PageCallback callback)
        {
            pageOpenCallback += callback;
        }

        // Remove a callback for when the page is opened.
        public void OnPageOpenedRemoveCallback(PageCallback callback)
        {
            pageOpenCallback -= callback;
        }

        // Add a callback for when the page is closd.
        public void OnPageClosedAddCallback(PageCallback callback)
        {
            pageCloseCallback += callback;
        }

        // Remove a callback for when the page is closed.
        public void OnPageClosedRemoveCallback(PageCallback callback)
        {
            pageCloseCallback -= callback;
        }

        // Called when the page is opened.
        public virtual void OnPageOpened()
        {
            // Trigger the callbacks.
            if (pageOpenCallback != null)
                pageOpenCallback();
        }

        // Called when the page is closed.
        public virtual void OnPageClosed()
        {
            // Trigger the callbacks.
            if (pageCloseCallback != null)
                pageCloseCallback();
        }
    }
}