

<script>
    document.addEventListener("DOMContentLoaded", function () {
        document.querySelector('.toggle-btn').addEventListener('click', function () {
            document.querySelector('.sidebar').classList.toggle('collapsed');
            document.querySelector('.main-content').classList.toggle('expanded');
        });
    });
</script>




    // Toggle dropdown arrows
    $('.nav-link[data-bs-toggle="collapse"]').on('click', function() {
        $(this).find('.fa-chevron-down').toggleClass('rotate-180');
    });

    // Add active class to current link
    const currentLocation = window.location.pathname;
    $('.nav-link').each(function() {
        if ($(this).attr('href') === currentLocation) {
            $(this).addClass('active');
        }
    });
});