import { ApolloServer } from '@apollo/server';
import { startStandaloneServer } from '@apollo/server/standalone';
import { buildSubgraphSchema } from '@apollo/subgraph';
import gql from 'graphql-tag';
import fetch from 'node-fetch';

const typeDefs = gql`
  type Hotel @key(fields: "id") {
    id: ID!
    name: String
    city: String
    stars: Int
  }

  type Query {
    hotelsByIds(ids: [ID!]!): [Hotel]
  }
`;

const resolvers = {
  Hotel: {
      __resolveReference: async ({ id }) => {
          const response = await fetch(`http://hotelio-monolith:8080/api/hotels/${id}`);
          if (!response.ok) {
              throw new Error(`Failed to fetch hotel ${id}: ${response.statusText}`);
          }
          return response.json();
      },
  },
  Query: {
    hotelsByIds: async (_, { ids }) => {
        const hotels = await Promise.all(
            ids.map(async (id) => {
                const response = await fetch(`http://hotelio-monolith:8080/api/hotels/${id}`);
                if (!response.ok) {
                    throw new Error(`Failed to fetch hotel ${id}: ${response.statusText}`);
                }
                return response.json();
            })
        );
        return hotels;
    },
  },
};

const server = new ApolloServer({
  schema: buildSubgraphSchema([{ typeDefs, resolvers }]),
});

startStandaloneServer(server, {
  listen: { port: 4002 },
}).then(() => {
  console.log('✅ Hotel subgraph ready at http://localhost:4002/');
});
