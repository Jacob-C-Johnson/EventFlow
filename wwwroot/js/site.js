const apiBaseUrl = ''; // Base URL (e.g., /api/eventflow) if needed, otherwise leave as an empty string

// Fetch and display all events
async function viewAllEvents() {
    try {
        const response = await fetch(`${apiBaseUrl}/GetAllEvents`);
        if (!response.ok) throw new Error('Failed to fetch events.');

        const data = await response.json();
        const events = data.Events || []; // Adjust structure if backend response differs

        let contentHtml = '<h2>All Events</h2><ul>';
        if (events.length === 0) {
            contentHtml += '<p>No events found.</p>';
        } else {
            events.forEach(event => {
                contentHtml += `
                    <li>
                        <strong>${event.Title}</strong> - ${event.EventLocation}
                        <p>${event.EventDescription}</p>
                        <p>Total Attendees: ${event.TotalAttendees}</p>
                    </li>`;
            });
        }
        contentHtml += '</ul>';

        document.getElementById('content').innerHTML = contentHtml;
    } catch (error) {
        console.error('Error fetching events:', error);
        document.getElementById('content').innerHTML = '<p>Error fetching events. Please try again later.</p>';
    }
}

// Display the Create Reservation form
function createReservation() {
    const formHtml = `
        <h2>Create a Reservation</h2>
        <form onsubmit="submitReservation(event)">
            <div>
                <label>Reservation Time</label>
                <input type="time" id="reservationTime" required />
            </div>
            <div>
                <label>Reservation Date</label>
                <input type="date" id="reservationDate" required />
            </div>
            <div>
                <label>User ID</label>
                <input type="number" id="userId" required />
            </div>
            <div>
                <label>Event ID</label>
                <input type="number" id="eventId" required />
            </div>
            <button type="submit">Create Reservation</button>
        </form>
    `;
    document.getElementById('content').innerHTML = formHtml;
}

// Submit a new reservation
async function submitReservation(event) {
    event.preventDefault();

    const reservation = {
        ReservationTime: document.getElementById('reservationTime').value,
        ReservationDate: document.getElementById('reservationDate').value,
        Status: 'Pending',
        UserId: parseInt(document.getElementById('userId').value, 10),
        EventId: parseInt(document.getElementById('eventId').value, 10)
    };

    try {
        const response = await fetch(`${apiBaseUrl}/AddReservation`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(reservation)
        });

        if (response.ok) {
            alert('Reservation created successfully!');
            document.getElementById('content').innerHTML = '';
        } else {
            const error = await response.json();
            alert(`Failed to create reservation: ${error.message}`);
        }
    } catch (error) {
        console.error('Error creating reservation:', error);
        alert('Error creating reservation. Please try again later.');
    }
}

// Fetch and display reservations for a user
async function viewMyReservations() {
    const userId = prompt('Enter your User ID:');
    if (!userId) return;

    try {
        const response = await fetch(`${apiBaseUrl}/GetReservations/${userId}`);
        if (!response.ok) throw new Error('Failed to fetch reservations.');

        const data = await response.json();
        const reservations = data.Reservations || []; // Adjust structure based on backend response

        let contentHtml = '<h2>My Reservations</h2><ul>';
        if (reservations.length === 0) {
            contentHtml += '<p>No reservations found.</p>';
        } else {
            reservations.forEach((reservation, index) => {
                contentHtml += `
                    <li>
                        ${index + 1}. 
                        <strong>Time:</strong> ${reservation.ReservationTime}, 
                        <strong>Date:</strong> ${reservation.ReservationDate}, 
                        <strong>Status:</strong> ${reservation.Status}
                    </li>`;
            });
        }
        contentHtml += '</ul>';

        document.getElementById('content').innerHTML = contentHtml;
    } catch (error) {
        console.error('Error fetching reservations:', error);
        document.getElementById('content').innerHTML = '<p>Error fetching reservations. Please try again later.</p>';
    }
}
