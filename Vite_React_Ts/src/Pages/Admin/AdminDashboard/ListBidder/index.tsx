import {
  List,
  ListItem,
  ListItemPrefix,
  Avatar,
  Card,
  Typography,
} from "@material-tailwind/react";
import React from "react";
import { useState, useEffect , useContext} from "react";
import {
  getUserListJoinAuction
} from "../../../../api/admindashboard";
import { UserContext } from "../../../../context/userContext";

const ListBidder: React.FC = () => {

  const { token } = useContext(UserContext);
  const [userData, setUserData] = useState<UserJoinAuction[]| undefined>(undefined);
  
  useEffect(() => {
    const fetchGetUserJoinAuction = async () => {
      try {
        if (token) {
          const data: UserJoinAuction[] | undefined = await getUserListJoinAuction(token);
          setUserData(data);
        }
      } catch (error) {
        console.error("Error fetching user list:", error);
      }
    };

    fetchGetUserJoinAuction();
  }, [token]);
  return (
    <>
      <Card className="w-full h-full">
        <List>
          {userData && userData.map((user) => (
            <ListItem key={user.accountId}>
              <ListItemPrefix>
                <Avatar
                  variant="circular"
                  alt={user.accountName}
                  src="https://docs.material-tailwind.com/img/face-2.jpg" // Đảm bảo rằng bạn có trường avatarUrl trong đối tượng UserJoinAuction
                />
              </ListItemPrefix>
              <div>
                <Typography variant="h6" color="blue-gray">
                  {user.accountName}
                </Typography>
                <Typography variant="small" color="gray" className="font-normal">
                  {user.accountEmail}
                </Typography>
                <Typography variant="small" color="gray" className="font-normal">
                  <span>Number of auctions participated: {user.numberOfAuction}</span>
                </Typography>
              </div>
            </ListItem>
          ))}
        </List>
      </Card>
    </>
  );
};

export default ListBidder;
