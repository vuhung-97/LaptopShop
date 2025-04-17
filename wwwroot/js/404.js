fetch('/animations/444.json')
    .then(response => response.json())
    .then(animationData => {
        bodymovin.loadAnimation({
            container: document.getElementById('svgContainer'),
            renderer: 'svg',
            loop: true,
            autoplay: true,
            animationData: animationData
        });
    })
    .catch(err => console.error("Lỗi khi tải animation:", err));
