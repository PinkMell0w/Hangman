-ñip-

##DB setup 
1. Open SQL Server Object Explorer in VS
2. Connect to localdb '\MSSQLLocalDB'
3. Run DB scripts -> '\database' from 01 to 03 (or 04, WIP)
4. Run seeds '\database\seed\'

---
# UC

| ID    | Name                      | Actor           | Service       | Done   |
|-------|---------------------------|-----------------|---------------|--------|
| CU-01 | Sign up                   | Anyone          | AuthService   | 🔧     | 
| CU-02 | Sign in                   | Anyone          | AuthService   | ❌     |
| CU-03 | Password recovery         | User            | AuthService   | ❌     |
| CU-04 | Check user stats          | User            | UserService   | ❌     |
| CU-05 | Customize profile         | User            | UserService   | ❌     |
| CU-06 | Search user               | Anyone          | AuthService   | ❌     |
| CU-07 | Send friend request       | User            | UserService   | ❌     |
| CU-08 | Remove friend             | User            | UserService   | ❌     |
| CU-09 | Create match              | User            | MatchService  | ❌     |
| CU-10 | Join match                | User            | MatchService  | ❌     |
| CU-11 | Guess                     | User            | MatchService  | ❌     |
| CU-12 | KickPlayer                | User            | MatchService  | ❌     |
| CU-13 | Win/Surrender             | Admin           | MatchService  | ❌     |
| CU-14 | Send message              | User            | MessageService| ❌     |

✅ Implemented · 🔧 Partial · ❌ Not started