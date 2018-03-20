//listen for any click
document.addEventListener("click", function (e) {
    if (Array.from(e.target.classList)[0] === "addComment") {
        const markerId = e.target.id;
        window.location = `/Comments/Create?mId=${markerId}`
    }
});