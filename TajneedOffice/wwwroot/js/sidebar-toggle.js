document.addEventListener('DOMContentLoaded', function () {
    const sidebar = document.getElementById('sidebar');
    const toggleBtn = document.getElementById('sidebarToggle');
    const mainContent = document.getElementById('mainContent');
    const sidebarChevron = document.getElementById('sidebarChevron');
    const themeToggle = document.getElementById('themeToggle');
    const themeIcon = document.getElementById('themeIcon');

    // Sidebar state
    const isInitiallyCollapsed = localStorage.getItem('sidebarCollapsed') === 'true';
    if (isInitiallyCollapsed) {
        sidebar?.classList.add('sidebar-collapsed');
        mainContent?.classList.add('expanded');
        sidebarChevron?.classList.replace('fa-chevron-right', 'fa-chevron-left');
    }

    if (sidebar && toggleBtn) {
        toggleBtn.addEventListener('click', function () {
            sidebar.classList.toggle('sidebar-collapsed');
            mainContent?.classList.toggle('expanded');
            
            if (sidebar.classList.contains('sidebar-collapsed')) {
                // If collapsed, show the icon to expand (chevron-left for RTL)
                sidebarChevron?.classList.replace('fa-chevron-right', 'fa-chevron-left');
            } else {
                // If expanded, show the icon to collapse (chevron-right for RTL)
                sidebarChevron?.classList.replace('fa-chevron-left', 'fa-chevron-right');
            }
            localStorage.setItem('sidebarCollapsed', sidebar.classList.contains('sidebar-collapsed'));
        });

        // Add hover effects for collapsed state
        if (sidebar) {
            sidebar.addEventListener('mouseenter', function() {
                if (sidebar.classList.contains('sidebar-collapsed')) {
                    sidebar.style.width = '200px';
                    sidebar.querySelectorAll('.menu-text').forEach(text => {
                        text.style.opacity = '1';
                        text.style.visibility = 'visible';
                    });
                }
            });

            sidebar.addEventListener('mouseleave', function() {
                if (sidebar.classList.contains('sidebar-collapsed')) {
                    sidebar.style.width = 'var(--sidebar-collapsed-width)';
                    sidebar.querySelectorAll('.menu-text').forEach(text => {
                        text.style.opacity = '0';
                        text.style.visibility = 'hidden';
                    });
                }
            });
        }
    }

    // Theme toggle
    function setTheme(isDark) {
        document.body.classList.toggle('dark-mode', isDark);
        themeIcon?.classList.toggle('fa-sun', isDark);
        themeIcon?.classList.toggle('fa-moon', !isDark);
        localStorage.setItem('theme', isDark ? 'dark' : 'light');
    }

    const isThemeDark = localStorage.getItem('theme') === 'dark';
    setTheme(isThemeDark);

    themeToggle?.addEventListener('click', () => {
        setTheme(!document.body.classList.contains('dark-mode'));
    });

    // Handle mobile responsiveness
    function handleMobileSidebar() {
        if (window.innerWidth <= 768) {
            sidebar?.classList.add('sidebar-collapsed');
            mainContent?.classList.add('expanded');
        }
    }

    // Initial mobile check
    handleMobileSidebar();

    // Listen for window resize
    window.addEventListener('resize', handleMobileSidebar);

    // Add smooth scrolling to sidebar
    if (sidebar) {
        sidebar.style.scrollBehavior = 'smooth';
    }

    // Add keyboard navigation support
    document.addEventListener('keydown', function(e) {
        if (e.ctrlKey && e.key === 'b') {
            e.preventDefault();
            toggleBtn?.click();
        }
    });

    // Add focus management for accessibility
    const navLinks = sidebar?.querySelectorAll('.nav-link');
    if (navLinks) {
        navLinks.forEach(link => {
            link.addEventListener('focus', function() {
                this.style.outline = '2px solid var(--primary-color)';
                this.style.outlineOffset = '2px';
            });
            
            link.addEventListener('blur', function() {
                this.style.outline = 'none';
            });
        });
    }
}); 