<?php
if ($_SERVER["REQUEST_METHOD"] == "POST") {
    // Collecting data from the form
    $name = htmlspecialchars($_POST['name']);
    $email = htmlspecialchars($_POST['email']);
    $age = htmlspecialchars($_POST['age']);

    // Displaying the collected data
    echo "<h2>Submitted Data:</h2>";
    echo "Name: " . $name . "<br>";
    echo "Email: " . $email . "<br>";
    echo "Age: " . $age . "<br>";
}
?>