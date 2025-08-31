import { ApolloServer } from '@apollo/server';
import { startStandaloneServer } from '@apollo/server/standalone';
import { buildSubgraphSchema } from '@apollo/subgraph';
import gql from 'graphql-tag';

import grpc from '@grpc/grpc-js';
import protoLoader from '@grpc/proto-loader';

const PROTO_PATH = './booking.proto';
const packageDefinition = protoLoader.loadSync(PROTO_PATH, {
  keepCase: true,
  longs: String,
  enums: String,
  defaults: true,
  oneofs: true,
});
const bookingProto = grpc.loadPackageDefinition(packageDefinition).booking;

const bookingClient = new bookingProto.BookingService(
  'booking-service:8080',
  grpc.credentials.createInsecure()
);

const typeDefs = gql`
  type Booking @key(fields: "id") {
    id: ID!
    userId: String!
    hotelId: String!
    promoCode: String
    discountPercent: Int
  }

  type Query {
    bookingsByUser(userId: String!): [Booking]
  }

`;

const resolvers = {
    Query: {
        bookingsByUser: async (_, { userId }, context) => {
            if (!context.userId) { throw new Error("Unauthorized"); }
            if (context.userId !== userId) {
                throw new Error("Forbidden: cannot access another user's bookings");
            }

            return new Promise((resolve, reject) => {
                bookingClient.ListBookings({ user_id: userId }, (err, response) => {
                    if (err) {
                        return reject(err);
                    }

                    resolve(
                        response.bookings.map((b) => ({
                            id: b.id,
                            userId: b.user_id,
                            hotelId: b.hotel_id,
                            promoCode: b.promo_code,
                            discountPercent: b.discount_percent,
                            price: b.price,
                            createdAt: b.created_at,
                        }))
                    );
                });
            });
        },
    },

    Booking: {
        __resolveReference: async (ref, context) => {
            if (!context.userId) { throw new Error("Unauthorized"); }
            return new Promise((resolve, reject) => {
                bookingClient.ListBookings({ user_id: context.userId }, (err, response) => {
                    if (err) {
                        return reject(err);
                    }

                    const booking = response.bookings.find((b) => b.id === ref.id);
                    if (!booking) {
                        return resolve(null);
                    }

                    resolve({
                        id: booking.id,
                        userId: booking.user_id,
                        hotelId: booking.hotel_id,
                        promoCode: booking.promo_code,
                        discountPercent: booking.discount_percent,
                        price: booking.price,
                        createdAt: booking.created_at,
                    });
                });
            });
        },
    },
};

const server = new ApolloServer({
  schema: buildSubgraphSchema([{ typeDefs, resolvers }]),
});

startStandaloneServer(server, {
  listen: { port: 4001 },
  context: async ({ req }) => {
      const userId = req.headers['userid'];
      return { userId };
  },
}).then(() => {
  console.log('✅ Booking subgraph ready at http://localhost:4001/');
});
