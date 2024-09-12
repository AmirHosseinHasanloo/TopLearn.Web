function changeGroup() {
    $("#form_Filter").submit();
}
function saveFilterOnChangePage(pageId) {
    $("#pageId").val(pageId);
    $("#form_Filter").submit();
}
$(function () {

    var slider = document.getElementById('priceFilter');
    noUiSlider.create(slider, {
        start: [0, 10000000],
        step: 5000,
        connect: true,
        direction: 'rtl',
        range: {
            'min': 0,
            'max': 10000000
        }
    });

    var marginMin = document.getElementById('min-text'),
        marginMax = document.getElementById('max-text');

    slider.noUiSlider.on('update', function (values, handle) {
        var xvalue = Math.round(values[handle]);
        if (handle) {
            marginMax.innerHTML = xvalue;
        } else {
            marginMin.innerHTML = xvalue;
        }
        //console.log(values[handle]);
    });

    slider.noUiSlider.on('change', function (values, handle) {
        var xvalue = Math.round(values[handle]);
        if (handle) {
            // setGetParameter('max_price', xvalue);
            $('#max-value').val(xvalue);
        } else {
            // setGetParameter('min_price', xvalue);
            $('#min-value').val(xvalue);
        }
    });


    slider.noUiSlider.on('slide', function (values, handle) {

        console.log(values[0]);
        console.log(values[1]);

        var xvalue = Math.round(values[handle]);
        //updateSliderRange( Math.round(values[0]), Math.round(values[1]) );
    });


    function updateSliderRange(min, max) {
        slider.noUiSlider.updateOptions({
            range: {
                'min': min,
                'max': max
            }
        });
    }
});