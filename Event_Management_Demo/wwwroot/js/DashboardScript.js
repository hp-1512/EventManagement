function getMonthName(monthNumber) {
    const date = new Date();
    date.setMonth(monthNumber - 1);

    // Using the browser's default locale.
    return date.toLocaleString([], { month: 'long' });
}


//-------------------------------------------------------------------------------------------------
//Pie chart for Events Status Summary
function DashboardData() {

    $.ajax({

        url: "/Methods/EventsDataPieChart",
        method: "GET",
        success: function (result) {
            console.log(result);
            new Chart("eventsPieChartContainer", {
                type: 'pie',
                data: {
                    labels: ["Completed Events", "Ongoing Events", "Upcoming Events"],
                    datasets: [{
                        backgroundColor: ["rgba(185, 29, 71, 0.7)", "rgba(0, 171, 169, 0.7)", "rgba(43, 87, 151, 0.7)"],
                        data: [result.completedEvents, result.ongoingEvents, result.upcomingEvents],
                    }]
                },
                options: {
                    plugins: {
                        title: {
                            display: true,
                            text: 'Insights Of Events'
                        },
                    },
                },
            });
        },
        error: function (error) {
            console.log(error);
        }
    });

    //-------------------------------------------------------------------------------------------------
    //Pie chart for Active-Inactive Users

    $.ajax({
        url: "/Methods/UsersDataPieChart",
        method: "GET",
        success: function (data) {
            console.log(data);
            new Chart("usersPieChartContainer", {
                type: 'pie',
                data: {
                    labels: ["Active Users", "Inactive Users"],
                    datasets: [{
                        backgroundColor: ['rgba(232, 195, 185, 0.7)', 'rgba(30, 113, 69, 0.7)'],
                        data: [data.activeUsers, data.inactiveUsers],
                    }]
                },
                options: {
                    plugins: {
                        title: {
                            display: true,
                            text: 'Insights Of Users'
                        },
                    },
                },
            });
        },
        error: function (error) {
            console.log(error);
        }
    });

    //-------------------------------------------------------------------------------------------------
    //Bar chart for Annual Summary for Events

    $.ajax({
        url: "/Methods/EventsDataBarChart",
        method: "GET",
        success: function (result) {
            console.log(result);
            var months = [];
            var values = [];
            $.each(result, function (key, item) {
                months.push(getMonthName(item.month));
                values.push(item.totalEvents);
            });
            var range = (Math.max.apply(null, values) + 1);
            new Chart("eventsBarChartContainer", {
                type: 'bar',
                data: {
                    labels: months,
                    datasets: [{
                        label: 'Number of Events Conducted',
                        data: values,
                        backgroundColor: [
                            'rgba(255, 99, 132, 0.2)',
                            'rgba(255, 159, 64, 0.2)',
                            'rgba(255, 205, 86, 0.2)',
                            'rgba(75, 192, 192, 0.2)',
                            'rgba(54, 162, 235, 0.2)',
                            'rgba(153, 102, 255, 0.2)',
                            'rgba(201, 203, 207, 0.2)',
                            'rgba(17, 46, 81, 0.2)',
                            'rgba(133, 58, 34, 0.2)',
                            'rgba(54, 158, 82, 0.2)',
                            'rgba(71, 11, 97, 0.2)',
                            'rgba(101, 112, 26, 0.2)',
                        ],
                        borderColor: [
                            'rgb(255, 99, 132)',
                            'rgb(255, 159, 64)',
                            'rgb(255, 205, 86)',
                            'rgb(75, 192, 192)',
                            'rgb(54, 162, 235)',
                            'rgb(153, 102, 255)',
                            'rgb(201, 203, 207)',
                            'rgb(17, 46, 81)',
                            'rgb(133, 58, 34)',
                            'rgb(54, 158, 82)',
                            'rgb(71, 11, 97)',
                            'rgb(101, 112, 26)',
                        ],
                        borderWidth: 1.3
                    }]
                },
                options: {
                    /*indexAxis: 'y',*/
                    scales: {
                        y: {
                            beginAtZero: true,
                            max: range,
                        }
                    },
                    plugins: {
                        legend: {
                            position: 'top',
                        },
                        title: {
                            display: true,
                            text: 'Annual Summary of Total Events Conducted'
                        }
                    }
                },


            });

        },
        error: function (error) {
            console.log(error);
        }
    });
}

