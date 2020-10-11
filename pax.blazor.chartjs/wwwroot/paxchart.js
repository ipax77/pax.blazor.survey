var isChartJSInit = false;

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

async function initiateChartJS(chartId) {
    if (isChartJSInit == false) {
        window.isLoading = true;
        isChartJSInit = true;
        window.myCharts = [];
        console.log("chartjs init");
        var ctx = document.getElementById("paxchartjscanvas" + chartId).getContext('2d');
        addScript("_content/pax.blazor.chartjs/Chart.min.js");
        var loading = true;
        await sleep(250);
        var i = 0;
        while (loading) {
            try {
                window.myChart = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: ['Red'],
                        datasets: [{
                            label: '# of Votes',
                            data: [12],
                            borderWidth: 1
                        }]
                    }
                });
                window.myChart.destroy();
                loading = false;
            } catch (err) {
                loading = true;
                i += 1;
                await sleep(125);
            }
            if (i > 50) {
                console.error("Failed loading Chart.min.js :(");
                return;
            }
        }
        addScript("_content/pax.blazor.chartjs/chartjs-plugin-datalabels.js");
        loading = true;
        await sleep(125);
        i = 0;
        while (loading) {
            try {
                var bab = ChartDataLabels;
                loading = false;
            } catch (err) {
                loading = true;
                i += 1;
                await sleep(75);
            }
            if (i > 50) {
                console.error("Failed loading chartjs-plugin-datalabels.js :(");
                return;
            }
        }
        //addScript("_content/pax.blazor.chartjs/chartjs-plugin-labels.min.js");
        //loading = true;
        //await sleep(125);
        window.isLoading = false;
    }
}

function addScript(filename) {
    var head = document.getElementsByTagName('body')[0];

    var script = document.createElement('script');
    script.src = filename;
    script.type = 'text/javascript';

    head.insertBefore(script, document.getElementsByTagName("script")[1]);
}

window.DrawChart = async (chartId) => {
    await initiateChartJS(chartId);
};

window.AddData = (chartId, label, winrate, dcolor, dimage) => {
    if (window.myCharts[chartId] != null) {
        if (window.myCharts[chartId].data.datasets.length == 1) {
            window.myCharts[chartId].data.labels.push(label);
            window.myCharts[chartId].data.datasets[window.myCharts[chartId].data.datasets.length - 1].data.push(winrate);
            window.myCharts[chartId].data.datasets[window.myCharts[chartId].data.datasets.length - 1].backgroundColor.push(dcolor);
            if (dimage > '') {
                window.myCharts[chartId].options.plugins.labels.images.push(JSON.parse(dimage));
            }
        } else {
            window.myCharts[chartId].data.datasets[window.myCharts[chartId].data.datasets.length - 1].data.push(winrate);
            window.myCharts[chartId].data.datasets[window.myCharts[chartId].data.datasets.length - 1].backgroundColor.push(dcolor);
        }
        window.myCharts[chartId].update();
    }
}

window.BarChart = async (chartId) => {
    var i = 0;
    while (window.isLoading) {
        i += 1;
        await sleep(125);
        if (i > 1000) {
            console.error("Failed init pie chart " + chartId);
            return false;
        }
    }
    var piecan = document.getElementById("paxchartjscanvas" + chartId);
    if (piecan != null) {
        var ctx = piecan.getContext('2d');
        var config = {
            plugins: [ChartDataLabels],
            type: 'horizontalBar',
                data: {
                labels: [],
                datasets: [
                    {
                        label: 'Result',
                        backgroundColor: [],
                        borderColor: "rgba(34, 38, 255, 1)",
                        pointBackgroundColor: "rgba(34, 38, 255, 1)",
                        borderWidth: 1,
                        data: [],
                        fill: true,
                        pointRadius: 5,
                        pointHoverRadius: 10,
                        showLine: true
                    }
                ]
            },
            options: {
                layout: {
                    padding: {
                        right: 50
                    }
                },
                plugins: {
                    datalabels: {
                        display: 'true',
                        color: '#0a050c',
                        //backgroundColor: '#cdc7ce',
                        //borderColor: '#491756',
                        //borderRadius: 4,
                        //borderWidth: 1,
                        formatter: function (value, context) {
                            return value + '%';
                        },
                        anchor: 'end',
                        align: 'start'
                    }
                },
                responsive: true,
                maintainAspectRatio: false,
                legend: {
                    display: false,
                    position: 'top',
                    labels: {
                        fontSize: 14,
                        fontColor: '#d9d8ea'
                    }
                },
                title: {
                    display: false,
                },
                scales: {
                    xAxes: [
                        {
                            scaleLabel: {
                                display: false,
                                labelString: '%',
                                fontColor: '#0a050c'
                            },
                            ticks: {
                                beginAtZero: true
                            }
                        }
                    ],
                    yAxes: [
                        {
                            scaleLabel: {
                                display: false,
                                labelString: 'Answers',
                                fontColor: '#0a050c'
                            }
                        }
                    ]
                }
            }
        }
        if(window.myCharts[chartId] != null) {
        window.myCharts[chartId].destroy();
        }
        window.myCharts[chartId] = new Chart(ctx, config);
        return true;
    }
}


window.PieChart = async(chartId) => {
    var i = 0;
    while (window.isLoading) {
        i += 1;
        await sleep(125);
        if (i > 1000) {
            console.error("Failed init pie chart " + chartId);
            return false;
        }
    }
    var piecan = document.getElementById("paxchartjscanvas" + chartId);
    if (piecan != null) {
        var ctx = piecan.getContext('2d');
        var config = {
            plugins: [ChartDataLabels],
            type: 'pie',
            data: {
                datasets: [{
                    data: [
                    ],
                    backgroundColor: [
                    ],
                    label: 'Result'
                }],
                labels: [
                ]
            },
            options: {
                layout: {
                    padding: {
                        left: 50,
                        right: 50,
                        top: 50,
                        bottom: 50
                    }
                },
                responsive: true,
                legend: {
                    display: false,
                    position: 'left',
                    labels: {
                        fontSize: 14,
                        fontColor: "#121202"
                    }
                },
                plugins: {
                    // Change options for ALL labels of THIS CHART
                    datalabels: {
                        display: 'auto',
                        color: '#0a050c',
                        backgroundColor: '#cdc7ce',
                        borderColor: '#491756',
                        borderRadius: 4,
                        borderWidth: 1,
                        formatter: function (value, context) {
                            return context.chart.data.labels[context.dataIndex] + "\n" + value + '%';
                        },
                        anchor: 'end',
                        align: 'end'
                    }
                }
            }

        };
        if (window.myCharts[chartId] != null) {
            window.myCharts[chartId].destroy();
        }
        window.myCharts[chartId] = new Chart(ctx, config);
        return true;
    }
}

window.RadarChart = async(chartId) => {
    var i = 0;
    while (window.isLoading) {
        i += 1;
        await sleep(125);
        if (i > 1000) {
            console.error("Failed init radar chart " + chartId);
            return false;
        }
    }
    var piecan = document.getElementById("paxchartjscanvas" + chartId);
    if (piecan != null) {
        var ctx = piecan.getContext('2d');
        var config = {
            plugins: [ChartDataLabels],
            type: "radar",
            data: {
                labels: [],
                datasets: [
                    {
                        label: "Result",
                        backgroundColor: [],
                        borderColor: "rgba(186, 13, 151,1)",
                        pointBackgroundColor: "rgba(186, 13, 151, 0.2)",
                        borderWidth: 1,
                        data: [],
                        fill: true,
                        pointRadius: 5,
                        pointHoverRadius: 10,
                        showLine: true,
                        datalabels: {
                            display: true,
                            color: '#0a050c',
                            backgroundColor: '#cdc7ce',
                            borderColor: '#491756',
                            borderRadius: 2,
                            borderWidth: 1,
                            formatter: function (value, context) {
                                return context.chart.data.labels[context.dataIndex] + "\n" + value + '%';
                            },
                            align: 'end',
                            anchor: 'center',
                        }
                    }
                ]
            },
            options: {
                layout: {
                    padding: {
                        left: 50,
                        right: 50,
                        top: 50,
                        bottom: 50
                    }
                },
                scale: {
                    ticks: {
                        color: "#808080",
                        backdropColor: "#faf88a",
                        display: true,
                        beginAtZero: true
                        //suggestedMax: 100
                    },
                    gridLines: {
                        color: "#808080",
                        lineWidth: 0.25
                    },
                    angleLines: {
                        display: true,
                        color: "#808080",
                        lineWidth: 0.25
                    },
                    pointLabels: {
                        display: false,
                        fontSize: 24,
                        fontColor: "#121202",
                        outsidePadding: 4,
                        textMargin: 4
                    }
                },
                responsive: true,
                maintainAspectRatio: true,
                legend: {
                    display: false
                },
                title: {
                    display: false
                },
                scales: {
                    yAxes: []
                },
            }
        }
        if (window.myCharts[chartId] != null) {
            window.myCharts[chartId].destroy();
        }
        window.myCharts[chartId] = new Chart(ctx, config);
        return true;
    }
}



window.DrawPieChart = (canvasId, mydata) => {
    google.charts.load('current', {
        packages: ['corechart', 'bar']
    });  
    google.charts.setOnLoadCallback(PopulationChart(mydata));  

    function PopulationChart(data) {
        var dataArray = [
            ['City', '2020 Population', '2010 Population', '2000 Population', '1990 Population']
        ];
        
        for (i = 0; i < dataArray[0].length; i++) {
            item = data[i];
            dataArray.push([item.cityName, item.populationYear2020, item.populationYear2010, item.populationYear2000, item.populationYear1990]);
        }
        console.log(JSON.stringify(dataArray));
        try {
            var data = google.visualization.arrayToDataTable(dataArray);
            var options = {
                title: 'Population of Largest Cities of Odisha ',
                chartArea: {
                    width: '50%'
                },
                colors: ['#b0120a', '#7b1fa2', '#ffab91', '#d95f02'],
                hAxis: {
                    title: 'City',
                    minValue: 0
                },
                vAxis: {
                    title: 'Total Population'
                }
            };
            var chart = new google.visualization.PieChart(document.getElementById('chart_div'));

            chart.draw(data, options);
        } catch { }
        return false;
    }  

}

window.exampleJsFunctions = {
  showPrompt: function (message) {
    return prompt(message, 'Type anything here');
  }
};

