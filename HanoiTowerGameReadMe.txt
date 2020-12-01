Projest notes.

HanoiTowerGame is RESTful Web API project implemented using C#, MS Visual Studio 2017, MS SQL Server Express.

Client should use 2 requests: Post to start game and Put to make disk move from one rod to anoter. Arguments are:
Post(int fromRod, int toRod), Put(int gameId, int fromRod, int toRod). Client should take in account that 0 corresponds to 1st rod, 1 to 2nd, and 2 to 3rd.

Server returns the following Json responce
{
    "Id": 26,
    "Definition": "{\"Rods\":[[1,2,3],[],[4]]}",
    "Message": null,
    "Status": 0
}
where
- Id is gameId which client shoud use in Put requests.
- Definition is serialized string of rods and disks state. Default state on start game is
"{\"Rods\":[[1,2,3,4],[],[]]}" when all disks are on 1st rode. After next move from 1st to 3rd the state is
"{\"Rods\":[[1,2,3],[],[4]]}". After next move from 1st to 2nd the state is
"{\"Rods\":[[1,2],[3],[4]]}". After next move from 3rd to 2nd the state is
"{\"Rods\":[[1,2],[3,4],[]]}", etc.
- Status has 3 values: 0 - success, 1 - failure, 2 - game completed.
- Message has error information if Status is 1.

After game is completed game record is removed from database.
