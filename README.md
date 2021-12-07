# PasswordProtectedApp
In this application I used Asp.Net.Identity library and updated it according to common security practices.
## Password Storage

### Asp.Net.Identity
This framework is commonly used for storing user credentials. It has a predefined set of tables it uses to store user data. Even though come of it's practices may seem outdated it can be customized easily.

### Password Hashing
To improve hashing algorythm in Identity, developer has to implement `IPasswordHasher` interface. it has methods for hashing the password and verifying the hashed password. My chosen hashing algorythm was Argon2id from Sodium.Core library. To enforce single responsibility and code reusability, two classes were created: first one used only hasing. The second one inherited the previous hasher and encrypted results of it's parent class.

### Peppering
To improve safety I encrypted hashes of the passwords. I used Salsa20 based algorythm from NaCl.Core Library. Same encryption scheme is used to provide sensitive information storage. it will be explained in pore detail later.

### Password Requirements
*Important notice! Password requirements validation was implemented only on front end part of the app. The developer understands that it should not be done in real life application, but he doesn't have enough time :(*
To make app more secure, weak passwords are prohibited for all users.

1 Password must have at least one of both capital and uppercase letters.
2 Password must conain numbers and special symbols.
3 Password strength meter must determine strength of at least 4 out of 5 to procceed with this password.

##### Password Strength Meter
I used `angular-password-strength-meter` library as a Strength Meter. this library uses a Dropbox developed algorithm for a realistic password strength estimator inspired by password crackers. This algorithm is packaged in a Javascript library called zxcvbn. In addition, the package contains a dictionary of commonly used English words, names and passwords. That's why it was decided to include it into the app.
