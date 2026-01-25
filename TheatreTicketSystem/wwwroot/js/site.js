// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener("DOMContentLoaded", function () {
    const btn = document.getElementById("chatbot-btn");
    const popup = document.getElementById("chatbot-popup");
    const closeBtn = document.getElementById("chatbot-close");
    const messages = document.getElementById("chatbot-messages");
    const input = document.getElementById("chatbot-text");
    const sendBtn = document.getElementById("chatbot-send");

    // If chatbot is not on this page/layout, do nothing
    if (!btn || !popup || !closeBtn || !messages || !input || !sendBtn) return;

    function addMessage(text, who) {
        const row = document.createElement("div");
        row.className = "msg " + who;

        const bubble = document.createElement("div");
        bubble.className = "bubble";
        bubble.textContent = text;

        row.appendChild(bubble);
        messages.appendChild(row);
        messages.scrollTop = messages.scrollHeight;
    }

    function toggle(open) {
        if (open) popup.classList.remove("chatbot-hidden");
        else popup.classList.add("chatbot-hidden");
    }

    btn.addEventListener("click", () => {
        const hidden = popup.classList.contains("chatbot-hidden");
        toggle(hidden);

        if (hidden && messages.children.length === 0) {
            addMessage("Hi! I’m the AI Assistant. Do you have any questions?.", "bot");
        }
        setTimeout(() => input.focus(), 50);
    });

    closeBtn.addEventListener("click", () => toggle(false));

    async function send() {
        const text = (input.value || "").trim();
        if (!text) return;

        addMessage(text, "user");
        input.value = "";
        sendBtn.disabled = true;

        try {
            const res = await fetch("/Chat/Ask", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ message: text })
            });

            if (!res.ok) {
                addMessage("Server error. Please try again.", "bot");
                return;
            }

            const data = await res.json();
            addMessage(data.reply || "No reply.", "bot");
        } catch (e) {
            addMessage("Network error. Please try again.", "bot");
        } finally {
            sendBtn.disabled = false;
            input.focus();
        }
    }

    sendBtn.addEventListener("click", send);
    input.addEventListener("keydown", (e) => {
        if (e.key === "Enter") send();
    });
});
