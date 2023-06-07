function getMonthName(monthNumber) {
    const date = new Date();
    date.setMonth(monthNumber - 1);

    // Using the browser's default locale.
    return date.toLocaleString([], { month: 'long' });
}

function DashboardData() {
    //-------------------------------------------------------------------------------------------------
    //Pie chart for Events Status Summary
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
                        backgroundColor: ["#b91d47","#00aba9","#2b5797"],
                        data: [result.completedEvents, result.ongoingEvents, result.upcomingEvents],
                    }]
                },
                options: {
                    
                    title: {
                        display: true,
                        text: 'Insights Of Events'
                    }

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
                        backgroundColor: ["#e8c3b9","#1e7145"],
                        data: [data.activeUsers, data.inactiveUsers],
                    }]
                },
                options: {

                    title: {
                        display: true,
                        text: 'Insights Of Users'
                    }

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
            new Chart("eventsBarChartContainer", {
                type: 'bar',
                data: {
                    labels: months,
                    datasets: [
                        {
                            label: 'Total Events',
                            data: values,
                            borderColor: 'rgb(255, 99, 132)',
                            backgroundColor: 'rgba(255, 99, 132, 0.5)',
                        }
                    ]
                },
                options: {
                    title: {
                        display: true,
                        text: 'Annual Report of Events Conducted'
                    },
                    indexAxis: 'y',
                    scales: {
                        y: {
                            beginAtZero: true,
                            max: 100,
                            ticks: {
                                stepSize: 10, // Adjust the step size of the ticks if needed
                            },
                        }
                    },
                    elements: {
                        bar: {
                            borderWidth: 2,
                        }
                    },
                    responsive: true,
                },
            });
           
        },
        error: function (error) {
            console.log(error);
        }
    });
}

