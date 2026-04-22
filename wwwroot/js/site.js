window.toggleStudentLink = function () {
    const roleSelect = document.getElementById("roleSelect");
    const block = document.getElementById("studentLinkBlock");

    if (!roleSelect || !block) {
        return;
    }

    const sync = () => {
        block.style.display = roleSelect.value === "User" ? "block" : "none";
    };

    roleSelect.addEventListener("change", sync);
    sync();
};
