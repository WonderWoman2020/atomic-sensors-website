db.createUser(
    {
        user: "backend",
        pwd: "backend",
        roles: [
            {
                role: "readWrite",
                db: "atomic-sensors"
            }
        ]
    }
);